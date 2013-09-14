package net.kibotu.sandbox.unity.android;

import android.util.Log;
import net.kibotu.sandbox.chat.client.android.SocketClient;
import org.jetbrains.annotations.NotNull;
import org.json.JSONArray;
import org.json.JSONObject;

public class SocketFacade {

    private static final String TAG = SocketFacade.class.getSimpleName();
    private static SocketClient socket;
    public static String url;

    public static void setUrl(@NotNull final String url) {
        SocketFacade.url = url;
    }

    public static void Emit(@NotNull final String name, @NotNull final String args) {
        try {
            if (socket == null) {
                socket = new SocketClient(url);
            }
            socket.client.emit(name, new JSONArray().put(new JSONObject(args)));
        } catch (Exception e) {
            Log.v(TAG, "Exception: " + e.getMessage());
            e.printStackTrace();
        }
    }
}
