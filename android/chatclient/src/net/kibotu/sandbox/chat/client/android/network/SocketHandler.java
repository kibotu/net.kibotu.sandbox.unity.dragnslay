package net.kibotu.sandbox.chat.client.android.network;

import com.koushikdutta.async.http.socketio.Acknowledge;
import com.koushikdutta.async.http.socketio.SocketIOClient;
import org.json.JSONArray;
import org.json.JSONObject;

public interface SocketHandler {

    public void EventCallback(final JSONArray argument, final Acknowledge acknowledge);

    public void StringCallback(final String message, final Acknowledge acknowledge);

    public void JSONCallback(final JSONObject jsonObject, final Acknowledge acknowledge);

    public void DisconnectCallback(final Exception e);

    public void ErrorCallback(final String error);

    public void ReconnectCallback();

    public void ConnectCallback(final Exception ex, final SocketIOClient client);
}
