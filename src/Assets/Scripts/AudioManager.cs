﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour {
    // Variables publicas

    // Variables privadas
    [SerializeField]
    GameObject _audioSourcePrefab;
    [SerializeField]
    AudioClip _music;
    [SerializeField]
    List<PairAudioClipString> _audioClips;

    private GameObject _currentAudioSource;
    // Metodos Awake, Start, Update....

    // Use this for spawn this instance
    void Awake(){
	
	}

	// Use this for initialization
	void Start () {
        _currentAudioSource = Instantiate(_audioSourcePrefab, transform.position, Quaternion.identity) as GameObject;
        _currentAudioSource.transform.parent = this.transform;
        _currentAudioSource.GetComponent<AudioSource>().clip = _music;
        _currentAudioSource.GetComponent<AudioSource>().loop = true;
        _currentAudioSource.GetComponent<AudioSource>().Play();
    }
	
	// Update is called once per frame

	// Otros métodos publicos
    public void Play(string clip)
    {
        _currentAudioSource = Instantiate(_audioSourcePrefab, transform.position, Quaternion.identity) as GameObject;
        _currentAudioSource.transform.parent = this.transform;
        _currentAudioSource.GetComponent<AudioSource>().clip = _audioClips.Find(e => e.StringKey == clip).AudioClip;
        _currentAudioSource.GetComponent<AudioSource>().Play();
        _currentAudioSource.GetComponent<AudioElement>().CheckActivity = true;
    }

    public void PlayHalfVolumen(string clip)
    {
        _currentAudioSource = Instantiate(_audioSourcePrefab, transform.position, Quaternion.identity) as GameObject;
        _currentAudioSource.transform.parent = this.transform;
        _currentAudioSource.GetComponent<AudioSource>().clip = _audioClips.Find(e => e.StringKey == clip).AudioClip;
        _currentAudioSource.GetComponent<AudioSource>().volume = 0.5f;
        _currentAudioSource.GetComponent<AudioSource>().Play();
        _currentAudioSource.GetComponent<AudioElement>().CheckActivity = true;
    }

    public void PlayThreeQuartersVolumen(string clip)
    {
        _currentAudioSource = Instantiate(_audioSourcePrefab, transform.position, Quaternion.identity) as GameObject;
        _currentAudioSource.transform.parent = this.transform;
        _currentAudioSource.GetComponent<AudioSource>().clip = _audioClips.Find(e => e.StringKey == clip).AudioClip;
        _currentAudioSource.GetComponent<AudioSource>().volume = 0.75f;
        _currentAudioSource.GetComponent<AudioSource>().Play();
        _currentAudioSource.GetComponent<AudioElement>().CheckActivity = true;
    }

    public void PlayCustomVolumen(string clip, float volume)
    {
        _currentAudioSource = Instantiate(_audioSourcePrefab, transform.position, Quaternion.identity) as GameObject;
        _currentAudioSource.transform.parent = this.transform;
        _currentAudioSource.GetComponent<AudioSource>().clip = _audioClips.Find(e => e.StringKey == clip).AudioClip;
        _currentAudioSource.GetComponent<AudioSource>().volume = volume;
        _currentAudioSource.GetComponent<AudioSource>().Play();
        _currentAudioSource.GetComponent<AudioElement>().CheckActivity = true;
    }


    // Otros metodos privados
}

[Serializable]
public class PairAudioClipString
{
    [SerializeField]
    public AudioClip AudioClip
    {
        get
        {
            //Some other code
            return _audioClip;
        }
        set
        {
            //Some other code
            _audioClip = value;
        }
    }

    [SerializeField]
    public string StringKey
    {
        get
        {
            //Some other code
            return _stringKey;
        }
        set
        {
            //Some other code
            _stringKey = value;
        }
    }
    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField]
    private string _stringKey;
}

