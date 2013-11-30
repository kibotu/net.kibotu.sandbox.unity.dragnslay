package net.kibotu.sandbox.network;

import net.kibotu.logger.Logger;
import org.apache.http.HttpResponse;
import org.apache.http.StatusLine;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONTokener;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

final public class JsonParser {

    private final static String TAG = JsonParser.class.getSimpleName();

    // utility class
    private JsonParser() {
    }

    /**
     * reads json by url
     *
     * @param url
     * @return JSONObject
     */
    @Nullable
    static public JSONObject readJson(final @NotNull String url) {

        StringBuilder builder = new StringBuilder();
        HttpClient client = new DefaultHttpClient();
        HttpGet httpGet = new HttpGet(url);
        JSONObject finalResult = null;
        try {
            HttpResponse response = client.execute(httpGet);
            StatusLine statusLine = response.getStatusLine();
            int statusCode = statusLine.getStatusCode();
            if (statusCode == 200) {
                BufferedReader reader = new BufferedReader(new InputStreamReader(response.getEntity().getContent(), "UTF-8"));
                String line;
                while ((line = reader.readLine()) != null) {
                    builder.append(line).append("\n");
                }
                finalResult = new JSONObject(new JSONTokener(builder.toString()));
            } else {
                Logger.e(TAG, "Failed to download status file.");
            }
        } catch (JSONException e) {
            Logger.e(TAG, e.getMessage());
        } catch (ClientProtocolException e) {
            Logger.e(TAG, e.getMessage());
        } catch (IOException e) {
            Logger.e(TAG, e.getMessage());
        }
        return finalResult;
    }
}