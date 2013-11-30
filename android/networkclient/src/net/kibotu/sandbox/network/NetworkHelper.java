package net.kibotu.sandbox.network;

import android.os.AsyncTask;
import net.kibotu.logger.Logger;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;
import org.json.JSONException;
import org.json.JSONObject;

public class NetworkHelper {

    public static final String TAG = NetworkHelper.class.getSimpleName();

    // utility class
    private NetworkHelper() {
    }

    public static void requestIpAddress(@NotNull final String url, @NotNull final AsyncTaskCallback<String> callback) {
        new AsyncTask<String, Integer, JSONObject>() {

            @Override
            protected void onPreExecute() {
                Logger.v(TAG, "Fetching ip");
            }

            @Override
            protected JSONObject doInBackground(@NotNull final String... jsonUrls) {
                return JsonParser.readJson(jsonUrls[0]);
            }

            @Override
            protected void onProgressUpdate(final Integer... progress) {
                publishProgress(1);
            }

            @Override
            protected void onPostExecute(@Nullable final JSONObject json) {

                // todo handle server offline state

                try {
                    callback.callback(json.getString(json.getString("network_interface")));
                } catch (final JSONException e) {
                    e.printStackTrace();
                }
            }

        }.execute(url);
    }
}
