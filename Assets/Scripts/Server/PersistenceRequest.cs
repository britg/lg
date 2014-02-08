using UnityEngine;
using System.Collections;
//using SimpleJSON;
using MiniJSON;

public class PersistenceRequest : LGMonoBehaviour {

	public static string authenticityToken = "";

	private string urlBase;

	public delegate void SuccessHandler(IDictionary response, GameObject receiver);
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

		formData.AddField("authenticity_token", PersistenceRequest.authenticityToken);
		formData.headers["Content-Type"] = "application/json";
		WWW request = new WWW(Endpoint(endpoint), formData);
		onSuccess = successHandler;
		onError = errorHandler;
		StartCoroutine(Request(request));
	}

	public void Put (string endpoint, WWWForm formData) {
		Put(endpoint, formData, LogResponse, LogResponse);
	}
	
	public void Put (string endpoint, WWWForm formData, SuccessHandler successHandler) {
		Put(endpoint, formData, successHandler, LogResponse);
	}
	
	public void Put (string endpoint, WWWForm formData,
	                  SuccessHandler successHandler, ErrorHandler errorHandler) {
		formData.AddField("authenticity_token", PersistenceRequest.authenticityToken);
		formData.AddField("_method", "PUT");
		formData.headers["Content-Type"] = "application/json";
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
		return Request (request, null);
	}
	
	protected IEnumerator Request (WWW request, GameObject receiver) {
		yield return request;
		if (request.error != null) {
			onError(request.error, receiver);
		} else {
			IDictionary response;
			if (request.text.Length > 0) {
				 response = (IDictionary) Json.Deserialize(request.text);
			} else {
				response = new Hashtable();
			}

			onSuccess(response, receiver);
		}
	}

	protected void LogResponse (IDictionary response, GameObject receiver) {
		Debug.Log("DB: Request response unhandled: " + response);
	}

	protected void LogResponse (string response, GameObject receiver) {
		Debug.Log("DB: Request response unhandled: " + response);
	}

	protected string Endpoint (string endpoint) {
		endpoint = urlBase + endpoint + ".json";
		return endpoint;
	}

}
