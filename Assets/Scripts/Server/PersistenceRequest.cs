using UnityEngine;
using System.Collections;
using MiniJSON;

public class PersistenceRequest : LGMonoBehaviour {

	public static string authenticityToken = "";

	public string urlBase;

	public delegate void SuccessHandler(Hashtable response, object receiver);
	public delegate void ErrorHandler(string response, object receiver);
	
	protected SuccessHandler onSuccess;
	protected ErrorHandler onError;

	void Awake () {
		urlBase = LG.dbHost + "/api";
	}

	public void Post (string endpoint, WWWForm formData) {
		Post(endpoint, formData, null, LogResponse, LogResponse);
	}
	
	public void Post (string endpoint, WWWForm formData, SuccessHandler successHandler) {
		Post(endpoint, formData, null, successHandler, LogResponse);
	}

	public void Post (string endpoint, WWWForm formData, object receiver, SuccessHandler successHandler) {
		Post(endpoint, formData, receiver, successHandler, LogResponse);
	}

	public void Post (string endpoint, WWWForm formData,
	                  SuccessHandler successHandler, ErrorHandler errorHandler) {
		Post (endpoint, formData, null, successHandler, errorHandler);
	}
	
	public void Post (string endpoint, WWWForm formData, object receiver,
	                  SuccessHandler successHandler, ErrorHandler errorHandler) {

		formData.AddField("authenticity_token", PersistenceRequest.authenticityToken);
		formData.headers["Content-Type"] = "application/json";
		WWW request = new WWW(Endpoint(endpoint), formData);
		onSuccess = successHandler;
		onError = errorHandler;
		StartCoroutine(Request(request, receiver));
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
		Get (endpoint, "", successHandler);
	}

	public void Get (string endpoint, string getParams, SuccessHandler successHandler) {
		Get(endpoint, getParams, successHandler, LogResponse);
	}

	public void Get (string endpoint, string getParams, object receiver, SuccessHandler successHandler) {
		Get(endpoint, getParams, receiver, successHandler, LogResponse);
	}
	
	public void Get (string endpoint, string getParams,
	                 SuccessHandler successHandler, ErrorHandler errorHandler) {
		Get (endpoint, getParams, null, successHandler, errorHandler);
	}

	public void Get (string endpoint, string getParams, object receiver,
	                 SuccessHandler successHandler, ErrorHandler errorHandler) {
		WWW request = new WWW(Endpoint(endpoint, getParams));
		onSuccess = successHandler;
		onError = errorHandler;
		StartCoroutine(Request(request, receiver));
	}

	protected IEnumerator Request (WWW request) {
		return Request (request, null);
	}
	
	protected IEnumerator Request (WWW request, object receiver) {
		yield return request;
		if (request.error != null) {
			onError(request.error, receiver);
		} else {
			Hashtable response = new Hashtable();
			if (request.text.Length > 0) {
				response = Json.Hashtable(request.text);
//				response["raw"] = request.text;
			} 

			onSuccess(response, receiver);
			request.Dispose();
		}
	}

	protected void LogResponse (Hashtable response, object receiver) {
		Debug.Log("DB: Request response unhandled: " + response);
	}

	protected void LogResponse (string response, object receiver) {
		Debug.Log("DB: Request response unhandled: " + response);
	}

	protected string Endpoint (string endpoint) {
		return Endpoint (endpoint, "");
	}

	protected string Endpoint (string endpoint, string getParams) {
		endpoint = urlBase + endpoint + ".json";
		if (getParams.Length > 0) {
			endpoint += "?" + getParams;
		}
		return endpoint;
	}

}
