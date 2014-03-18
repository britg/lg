using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDMessager : DisplayBehaviour {

	private static HUDMessager instance;
	public Queue queue = new Queue();

	public static HUDMessager Instance {
		get {
			if (instance == null) {
				instance = GameObject.Find("HUDMessager").GetComponent<HUDMessager>();
            }
            return instance;
        }
    }

	private static GameObject _messageTemplate;
	public static GameObject messageTemplate {
		get {
			if (_messageTemplate == null) {
				_messageTemplate = GameObject.Find("HUDMessage");
			}
			return _messageTemplate;
		}
	}

	private static HUDText _display;
	public static HUDText display {
		get {
			if (_display == null) {
				_display = Instance.GetHUDText(messageTemplate);
			}
			return _display;
		}
	}

	static bool shouldProcessNext = true;

    public void OnApplicationQuit () {
        instance = null;
    }

	public static void Queue (string text) {
		Queue (new HUDMessage(text));
	}

	public static void Queue (HUDMessage message) {
		Instance.queue.Enqueue(message);
	}

	void Start () {
		InvokeRepeating("ProcessQueue", 0.1f, 0.1f);
		NotificationCenter.AddObserver(this, LG.n_playerStatsLoaded);
	}

	void Update () {

	}

	void ProcessQueue () {
		if (shouldProcessNext && Instance.queue.Count > 0) {
			HUDMessage next = (HUDMessage)Instance.queue.Dequeue();
			Instance.ProcessMessage(next);
		}
	}

	void ProcessMessage (HUDMessage message) {
		HoldQueue();
		display.Add (message.text, message.color, message.duration);
		Invoke ("ResumeQueue", message.duration);
	}

	void HoldQueue () {
		shouldProcessNext = false;
	}

	void ResumeQueue () {
		shouldProcessNext = true;
	}

	void OnPlayerStatsLoaded () {
		UIFollowTarget follow = GetFollow(messageTemplate);
		follow.target = player.transform;
		follow.enabled = true;
		messageTemplate.SetActive(true);
	}
}
