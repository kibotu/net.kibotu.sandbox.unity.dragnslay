package net.kibotu.sandbox.chat.client.android.network;

import com.koushikdutta.async.http.socketio.Acknowledge;
import com.koushikdutta.async.http.socketio.SocketIOClient;
import net.kibotu.sandbox.chat.client.android.ChatClient;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class AndroidSocketHandler implements SocketHandler {

    @Override
    public void EventCallback(final JSONArray argument, final Acknowledge acknowledge) {
        try {
            JSONObject json = argument.getJSONObject(0);
            if(json.get("message").equals("Welcome!")) {
                ChatClient.uid = (String) json.get("uid");
                JSONObject getJsonObject = SocketClient.getJsonObject(ChatClient.uid, "uid", ChatClient.uid);
                SocketClient.Emit("add-user", getJsonObject);
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
        ChatClient.appendText("A "+argument.toString());
    }

    @Override
    public void StringCallback(final String message, final Acknowledge acknowledge) {
        ChatClient.appendText("S "+message);
    }

    @Override
    public void JSONCallback(final JSONObject jsonObject, final Acknowledge acknowledge) {
        ChatClient.appendText("J "+jsonObject);
    }

    @Override
    public void DisconnectCallback(final Exception e) {
        ChatClient.appendText("Disconnected " + ((e != null) ? e.getMessage() : ""));
    }

    @Override
    public void ErrorCallback(final String error) {
        ChatClient.appendText("Error " + error);
    }

    @Override
    public void ReconnectCallback() {
        ChatClient.appendText("Reconnect");
    }

    @Override
    public void ConnectCallback(final Exception ex, final SocketIOClient client) {
        ChatClient.appendText("Connected " + (ex == null ? "" : ex.getMessage()));
    }
}
