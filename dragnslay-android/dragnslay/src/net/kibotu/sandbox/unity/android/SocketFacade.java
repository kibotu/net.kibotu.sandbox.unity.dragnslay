package net.kibotu.sandbox.unity.android;

import android.util.Log;
import net.kibotu.sandbox.chat.client.android.SocketClient;
import org.jetbrains.annotations.NotNull;

public class SocketFacade {

    private static final String TAG = SocketClient.class.getSimpleName();
    private SocketClient socket;

    private static SocketFacade instance = new SocketFacade();

    public SocketFacade() {
        socket = new SocketClient();
    }

    public static void Emit(@NotNull String name, @NotNull String message) {
        Log.v(name, message);
        instance.socket.url = "http://172.16.3.13";
        try {
            instance.socket.testMessageToChat();
        } catch (Exception e) {
            Log.v(TAG, "Exception: " + e.getMessage());
            e.printStackTrace();
        }
    }
}
