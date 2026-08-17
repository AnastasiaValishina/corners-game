using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
	[SerializeField] AudioClip _buttonClick;
	[SerializeField] AudioClip _pawnClick;
	[SerializeField] AudioClip _pieceSlide;
	[SerializeField] AudioClip _win;
	[SerializeField] AudioSource _musicSource;

	[SerializeField] float soundVolume = 1f;

	public static AudioPlayer Instance { get; private set; }

	private Vector3 _cameraPos;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}

	private void Start()
	{
		_cameraPos = Camera.main.transform.position;

		//if (_musicSource != null)
		//{
		//	_musicSource.mute = !Settings.IsMusicOn;
		//}
	}

	public void ToggleMusic()
	{
		//Settings.IsMusicOn = !Settings.IsMusicOn;

		if (_musicSource != null)
		{
			//_musicSource.mute = !Settings.IsMusicOn;
		}
	}

	public void ToggleSound()
	{
		//Settings.IsSoundOn = !Settings.IsSoundOn;
	}

	public void PlayButtonClick() => PlayClip(_buttonClick, soundVolume);
	public void PlayPawnClick() => PlayClip(_pawnClick, soundVolume);
	public void PlaySlideSound() => PlayClip(_pieceSlide, soundVolume);
	public void PlayWinSound() => PlayClip(_win, soundVolume);

	void PlayClip(AudioClip clip, float volume)
	{
		//if (clip != null && Settings.IsSoundOn)
		if (clip != null)
		{
			AudioSource.PlayClipAtPoint(clip, _cameraPos, volume);
		}
	}
}
