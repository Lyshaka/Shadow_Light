using Codice.CM.Common.Tree;
using UnityEngine;

public class ControllerLight : MonoBehaviour
{
	[HideInInspector] public static ControllerLight Instance { get; private set; }

	// Public properties
	[Header("Movement")]
	[SerializeField] float moveSpeedMax = 10f;
	[SerializeField] float moveSpeedMaxAttached = 4f;
	[SerializeField] float acceleration = 10f;
	[SerializeField] float deceleration = 10f;


	[Header("Grow")]
	[SerializeField] float growDuration = 0.25f;
	[SerializeField] AnimationCurve growCurve;
	[SerializeField] float growMultiplicator = 1.5f;
	[SerializeField] Light pointLight;
	[SerializeField] TrailRenderer trailRenderer;

	[Header("Attach")]
	[SerializeField] float attachDuration = 0.1f;
	[SerializeField] AnimationCurve attachCurve;

	[Header("Technical")]
	[SerializeField] Transform mesh;
	[SerializeField] LightableManager lightableManager;


	// Private properties
	Rigidbody rb;

	// Grow
	Vector3 _originalSize;
	float _originalLightRange;
	float _originalLightIntensity;
	float _growElapsedTime;
	float _originalTrailSize;
	bool _isGrowing;
	bool _isGrown;

	// Attach
	[HideInInspector] public Transform objToAttach;
	Rigidbody _attachRb;
	Vector3 _attachOriginalPosition;
	Vector3 _attachPoint;
	float _attachElapsedTime;
	bool _isAttached;
	bool _isAttaching;
	



	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		_originalSize = mesh.localScale;
		_originalLightRange = pointLight.range;
		_originalLightIntensity = pointLight.intensity;
		_originalTrailSize = trailRenderer.startWidth;
	}

	void FixedUpdate()
	{
		HandleMove();
		HandleGrow();
		HandleAttach();
	}

	void HandleMove()
	{
		Vector3 inputVelocity = new Vector3(InputActionLight.Instance.Move.x, InputActionLight.Instance.Move.y, 0f).normalized;
		if (inputVelocity.sqrMagnitude > 0f && !_isGrown && !_isAttaching)
		{
			rb.linearVelocity += acceleration * Time.fixedDeltaTime * inputVelocity;
			rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, _isAttached ? moveSpeedMaxAttached : moveSpeedMax);
		}
		else
		{
			if (rb.linearVelocity.magnitude > 0.1f)
			{
				rb.linearVelocity = rb.linearVelocity.normalized * Mathf.Max(0f, rb.linearVelocity.magnitude - deceleration * Time.fixedDeltaTime);
			}
			else
			{
				rb.linearVelocity = Vector3.zero;
			}
		}

		if (_attachRb != null)
			_attachRb.MovePosition(rb.position);
	}

	void HandleGrow()
	{
		if (InputActionLight.Instance.Grow > 0f)
		{
			if (_growElapsedTime < growDuration)
			{
				_growElapsedTime += Time.deltaTime;
				_isGrowing = true;
				_isGrown = true;
			}
			else
			{
				_growElapsedTime = growDuration;
				_isGrowing = false;
			}
		}
		else
		{
			if (_growElapsedTime > 0f)
			{
				_growElapsedTime -= Time.deltaTime;
				_isGrowing = true;
				_isGrown = false;
			}
			else
			{
				_growElapsedTime = 0f;
				_isGrowing = false;
			}
		}

		if (_isGrowing)
		{
			SetLightSize(_growElapsedTime / growDuration);
		}
	}

	void HandleAttach()
	{
		if (InputActionLight.Instance.Attach)
		{
			InputActionLight.Instance.Attach = false;
			if (objToAttach != null && !_isAttaching)
			{
				if (_isAttached)
				{
					//objToAttach.parent = null;
					_attachRb = null;
					_isAttached = false;
				}
				else
				{
					//Debug.Log("Attach !");
					_attachOriginalPosition = transform.position;
					_attachPoint = objToAttach.GetComponent<Attachable>().AttachPoint;
					_attachElapsedTime = 0f;
					_isAttaching = true;
				}
			}
		}

		if (_isAttaching)
			Attach();
	}

	void SetLightSize(float percentage)
	{
		float mul = Mathf.Lerp(1f, growMultiplicator, percentage);

		mesh.localScale = _originalSize * mul;
		pointLight.range = _originalLightRange * mul * mul;
		pointLight.intensity = _originalLightIntensity * mul* mul;
		trailRenderer.startWidth = _originalTrailSize * mul;
		lightableManager.SetColliderSize(pointLight.range / 2f);
	}

	void Attach()
	{
		if (_attachElapsedTime < attachDuration)
		{
			rb.MovePosition(Vector3.Lerp(_attachOriginalPosition, _attachPoint, attachCurve.Evaluate(_attachElapsedTime / attachDuration)));
			_attachElapsedTime += Time.deltaTime;
		}
		else
		{
			rb.MovePosition(_attachPoint);
			_isAttached = true;
			_isAttaching = false;
			//objToAttach.parent = transform;
			_attachRb = objToAttach.GetComponentInParent<Rigidbody>();
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ResetSingleton()
	{
		Instance = null;
	}

}
