using UnityEngine;
using System.Collections;

public class PersistenceRequest : MonoBehaviour {

	private string urlBase;

	public delegate void SuccessHandler(string response, GameObject receiver);
	public delegate void ErrorHandler(string response, GameObject receiver);
	
	protected SuccessHandler onSuccess;
	protected ErrorHandler onError;

	void Awake () {
		urlBase = LG.dbHost;
	}

	public void Post (string endpoint, WWWForm formData) {
		Post(endpoint, formData, LogResponse, LogResponse);
	}
	
	public void Post (string endpoint, WWWForm formData, SuccessHandler successHandler) {
		Post(endpoint, formData, successHandler, LogResponse);
	}
	
	public void Post (string endpoint, WWWForm formData,
	                  SuccessHandler successHandler, ErrorHandler errorHandler) {
		WWW request = new WWW(Endpoint(endpoint), formData);
		onSuccess = successHandler;
		onError = errorHandler;
		StartCoroutine(Request(request));
	}
	
	public void Get (string endpoint, SuccessHandler successHandler) {
		Get(endpoint, successHandler, LogResponse);
	}
	
	public void Get (string endpoint,
	                 SuccessHandler successHandler, ErrorHandler errorHandler) {
		WWW request = new WWW(Endpoint(endpoint));
		onSuccess = successHandler;
		onError = errorHandler;
		StartCoroutine(Request(request));
	}

	protected IEnumerator Request (WWW request) {
		yield return request;
		if (request.error != null) {
			onError(request.error, null);
		} else {
			onSuccess(request.text, null);
		}
	}
	
	protected IEnumerator Request (WWW request, GameObject receiver) {
		yield return request;
		if (request.error != null) {
			onError(request.error, receiver);
		} else {
			onSuccess(request.text, receiver);
		}
	}
	
	protected void LogResponse (string response, GameObject receiver) {
		Debug.Log("Request response unhandled: " + response);
	}

	protected string Endpoint (string endpoint) {
		endpoint = urlBase + endpoint;
		return endpoint;
	}

}
