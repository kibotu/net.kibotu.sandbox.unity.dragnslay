package net.kibotu.sandbox.chat.client.android.network;

import android.util.Log;
import com.koushikdutta.async.http.AsyncHttpClient;
import com.koushikdutta.async.http.socketio.*;
import net.kibotu.sandbox.chat.client.android.logging.Logger;
import org.jetbrains.annotations.NotNull;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.net.InetAddress;
import java.util.concurrent.ExecutionException;

/**
 * using http://koush.com/AndroidAsync
 */
public class SocketClient {

    private static final String TAG = SocketClient.class.getSimpleName();
    private static SocketIOClient socket;
    private static SocketHandler socketHandler;

    public static void init(@NotNull final SocketHandler socketHandler) {
        SocketClient.socketHandler = socketHandler;
    }

    public static void connect(@NotNull final String host, final int port) {
        if (socket != null && socket.isConnected())
            return;
        if (socketHandler == null)
            throw new IllegalStateException("No SocketHandler defined. Please use init() before.");


        SocketIORequest req = null;
        try {
            req = new SocketIORequest("http://" + InetAddress.getByName(host).getHostAddress() + ":" + port);
            req.setLogging("Socket.IO", Log.VERBOSE);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            socket = SocketIOClient.connect(AsyncHttpClient.getDefaultInstance(), req, new ConnectCallback() {

                @Override
                public void onConnectCompleted(Exception ex, SocketIOClient client) {

                    socketHandler.ConnectCallback(ex, client);

                    client.addListener("message", new EventCallback() {
                        @Override
                        public void onEvent(final JSONArray argument, final Acknowledge acknowledge) {
                            socketHandler.EventCallback(argument, acknowledge);
                        }
                    });

                    client.setStringCallback(new StringCallback() {
                        @Override
                        public void onString(String message, Acknowledge acknowledge) {
                            acknowledge.acknowledge(new JSONArray().put(message));
                            socketHandler.StringCallback(message, acknowledge);
                        }
                    });

                    client.setJSONCallback(new JSONCallback() {
                        @Override
                        public void onJSON(final JSONObject jsonObject, final Acknowledge acknowledge) {
                            acknowledge.acknowledge(jsonObject.names());
                            socketHandler.JSONCallback(jsonObject, acknowledge);
                        }
                    });

                    client.setDisconnectCallback(new DisconnectCallback() {
                        @Override
                        public void onDisconnect(final Exception e) {
                            socketHandler.DisconnectCallback(e);
                        }
                    });

                    client.setErrorCallback(new ErrorCallback() {
                        @Override
                        public void onError(final String error) {
                            socketHandler.ErrorCallback(error);
                        }
                    });

                    client.setReconnectCallback(new ReconnectCallback() {
                        @Override
                        public void onReconnect() {
                            socketHandler.ReconnectCallback();
                        }
                    });
                }
            }).get();
        } catch (InterruptedException | ExecutionException e) {
            e.printStackTrace();
        }
    }

    public static void disconnect() {
        if (socket == null || !socket.isConnected()) return;
        socket.disconnect();
    }

    public static void reconnect() {
        if (socket == null || socket.isConnected()) return;
        socket.reconnect();
    }

    public static void Emit(@NotNull final String name, @NotNull final JSONArray args) {
        if (socket == null || !socket.isConnected()) return;
        Logger.v("emit", ""+args);
        socket.emit(name, args);
    }

    public static void Emit(@NotNull final String name, @NotNull final JSONObject args) {
        Emit(name, new JSONArray().put(args));
    }

    public static void Emit(@NotNull final String name, @NotNull final String args) {
        try {
            Emit(name, new JSONObject(args));
        } catch (JSONException e) {
            Log.v(TAG, "Exception: " + e.getMessage());
            e.printStackTrace();
        }
    }

    public static SocketIOClient socket() {
        return socket;
    }

    public static JSONObject getJsonObject(String name, String message) {

        JSONObject jObject = new JSONObject();
        try {
            jObject.put("name", "message");
            jObject.put("message", message).put("username", name);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return jObject;
    }

    public static JSONObject getJsonObject(final String name, final String ... msg) {

        if(msg.length % 2 != 0)
            throw new IllegalArgumentException("must be key, value pairs " + msg.length);

        JSONObject jObject = new JSONObject();
        try {
            jObject.put("name", "message");
            for(int i = 0; i < msg.length; i+=2)
                jObject.put(msg[i], msg[i+1]);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return jObject;
    }
}
