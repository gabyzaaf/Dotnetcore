package com.example.fabiengamel.candidatemanagement.Requests;

import com.android.volley.AuthFailureError;
import com.android.volley.NetworkResponse;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyLog;
import com.android.volley.toolbox.HttpHeaderParser;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.UnsupportedEncodingException;
import java.util.HashMap;
import java.util.Map;

/**
 * Created by Fabien gamel on 06/06/2017.
 */
public class LoginRequest extends JsonObjectRequest {

    private static final String LOGIN_REQUEST_URL ="http://192.168.1.17:5000/api/user/admin/auth/";
    Map<String, String> headers;
    JSONObject jsonBody;
    String requestBody;


    public LoginRequest(String mail, String password,Response.Listener<JSONObject> listener,Response.ErrorListener errorListener) throws JSONException {
        super(Request.Method.POST, LOGIN_REQUEST_URL, null, listener, errorListener);

        jsonBody = new JSONObject();
        jsonBody.put("email", mail);
        jsonBody.put("password", password);
        requestBody = jsonBody.toString();

        headers = new HashMap<>();
        headers.put("Content-Type", "application/json");
    }

    @Override
    public Map<String, String> getHeaders() throws AuthFailureError {
        return headers;
    }

    @Override
    public byte[] getBody() /*throws AuthFailureError */{
        try {
            return requestBody == null ? null : requestBody.getBytes("utf-8");
        } catch (UnsupportedEncodingException uee) {
            VolleyLog.wtf("Unsupported Encoding while trying to get the bytes of %s using %s", requestBody, "utf-8");
            return null;
        }
    }

  /*  @Override
    protected Response<JSONObject> parseNetworkResponse(NetworkResponse response) {
        String responseString = "";
        if (response != null) {
            responseString = String.valueOf(response.statusCode);
            // can get more details such as response.headers
        }
        return Response.success(responseString, HttpHeaderParser.parseCacheHeaders(response));
    }*/
}

