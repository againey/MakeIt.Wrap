/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace MakeIt.Wrap.Samples
{
	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class ShipController : MonoBehaviour
	{
		public Sprite driftingSprite;
		public Sprite thrustingSprite;
		public Laser laserPrefab;

		public Transform laserBarrel;
		public Transform lasers;

		public float thrustForce = 1f;
		public float maxSpeed = 1f;
		public float rotationSpeed = 1f;

		public float laserCooldownDuration = 1f;
		public float laserVelocity = 10f;

		private SpriteRenderer _spriteRenderer;
		private Rigidbody2D _rigidbody;

		private bool _thrusting = false;
		private float _orientationAngle = 0f;

		private float _laserCooldown = 0f;

		protected void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_rigidbody = GetComponent<Rigidbody2D>();

			_laserCooldown = laserCooldownDuration;

			_rigidbody.rotation = _orientationAngle;
		}

		protected void FixedUpdate()
		{
			_thrusting = Input.GetAxisRaw("Vertical") > 0f;

			var thrust = Input.GetAxis("Vertical");
			var rotation = Input.GetAxis("Horizontal");

			if (thrust > 0f)
			{
				_rigidbody.AddRelativeForce(Vector2.up * thrust * thrustForce);
			}

			var currentSpeed = _rigidbody.velocity.magnitude;
			if (currentSpeed > maxSpeed)
			{
				_rigidbody.velocity = (_rigidbody.velocity / currentSpeed) * maxSpeed;
			}

			if (rotation != 0f)
			{
				_orientationAngle = Mathf.Repeat(_orientationAngle - rotation * rotationSpeed, 360f);
				_rigidbody.rotation = _orientationAngle;
			}

			_laserCooldown = Mathf.Max(0f, _laserCooldown - Time.fixedDeltaTime);

			if (_laserCooldown == 0f && (Input.GetButton("Jump") || Input.GetButton("Fire1")))
			{
				var laser = Instantiate(laserPrefab);
				laser.transform.position = laserBarrel.transform.position;
				laser.transform.rotation = laserBarrel.transform.rotation;
				laser.transform.SetParent(lasers, true);
				var laserRigidbody = laser.GetComponent<Rigidbody2D>();
				laserRigidbody.velocity = laserBarrel.transform.up * laserVelocity;

				_laserCooldown = laserCooldownDuration;
			}
		}

		protected void Update()
		{
			if (_thrusting)
			{
				_spriteRenderer.sprite = thrustingSprite;
			}
			else
			{
				_spriteRenderer.sprite = driftingSprite;
			}
		}

		public void Die()
		{
			Respawn();
		}

		public void Respawn()
		{
			_thrusting = false;
			_orientationAngle = 0f;

			_laserCooldown = laserCooldownDuration;

			_rigidbody.position = Vector3.zero;
			_rigidbody.rotation = _orientationAngle;

			_rigidbody.velocity = Vector2.zero;

			_spriteRenderer.sprite = driftingSprite;
		}
	}
}
