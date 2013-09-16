package net.kibotu.sandbox.unity.android;

import android.util.Log;
import com.koushikdutta.async.http.socketio.*;
import com.unity3d.player.UnityPlayer;
import net.kibotu.sandbox.chat.client.android.SocketClient;
import org.jetbrains.annotations.NotNull;
import org.json.JSONArray;
import org.json.JSONObject;

import java.util.concurrent.ExecutionException;

/**
 * send message to unity: UnityPlayer.UnitySendMessage("GameObjectName1", "MethodName1", "Message to send");
 */
public class SocketHandler {

    private static final String TAG = SocketHandler.class.getSimpleName();
    private static SocketClient socket;

    public SocketHandler() {
    }

    private static void initHandler() {

        socket.client.on("send", new EventCallback() {
            @Override
            public void onEvent(final JSONArray argument, final Acknowledge acknowledge) {
                Log.v(TAG, "onEvent 'send' ack = " + acknowledge.toString() + " args = " + argument.toString());
                UnityPlayer.UnitySendMessage("SocketHandler", "StringCallback", argument.toString());
            }
        });

        socket.client.on("message", new EventCallback() {
            @Override
            public void onEvent(final JSONArray argument, final Acknowledge acknowledge) {
                Log.v(TAG, "onEvent 'message' ack = " + acknowledge.toString() + " args = " + argument.toString());
                UnityPlayer.UnitySendMessage("SocketHandler", "StringCallback", argument.toString());
            }
        });

        socket.client.setStringCallback(new StringCallback() {
            @Override
            public void onString(String message, Acknowledge acknowledge) {
                Log.v(TAG, "onString " + message + " " + acknowledge);
                acknowledge.acknowledge(new JSONArray().put(message));
                UnityPlayer.UnitySendMessage("SocketHandler", "StringCallback", message);
            }
        });

        socket.client.setJSONCallback(new JSONCallback() {
            @Override
            public void onJSON(final JSONObject jsonObject, final Acknowledge acknowledge) {
                Log.v(TAG, "onJson " + jsonObject + " " + acknowledge);
                acknowledge.acknowledge(jsonObject.names());
                UnityPlayer.UnitySendMessage("SocketHandler", "JSONCallback", jsonObject.toString());
            }
        });

        socket.client.setDisconnectCallback(new DisconnectCallback() {
            @Override
            public void onDisconnect(final Exception e) {
                Log.v(TAG, "onDisconnect " + e.getMessage());
                UnityPlayer.UnitySendMessage("SocketHandler", "DisconnectCallback", e.getMessage());
            }
        });

        socket.client.setErrorCallback(new ErrorCallback() {
            @Override
            public void onError(final String error) {
                Log.v(TAG, "onError " + error);
                UnityPlayer.UnitySendMessage("SocketHandler", "ErrorCallback", error);
            }
        });

        socket.client.setReconnectCallback(new ReconnectCallback() {
            @Override
            public void onReconnect() {
                Log.v(TAG, "onReconnect");
                UnityPlayer.UnitySendMessage("SocketHandler", "ReconnectCallback", null);
            }
        });
    }

    public static void connect(@NotNull final String url) {
        try {
            socket = new SocketClient(url);
            initHandler();
        } catch (ExecutionException e) {
            Log.v(TAG, "Exception: " + e.getMessage());
            e.printStackTrace();
        } catch (InterruptedException e) {
            Log.v(TAG, "Exception: " + e.getMessage());
            e.printStackTrace();
        }
    }

    public static void Emit(@NotNull final String name, @NotNull final String args) {
        try {
            socket.client.emit(name, new JSONArray().put(new JSONObject(args)));
            Log.v(TAG, "Emit: " + name);
        } catch (Exception e) {
            Log.v(TAG, "Exception: " + e.getMessage());
            e.printStackTrace();
        }
    }
}
