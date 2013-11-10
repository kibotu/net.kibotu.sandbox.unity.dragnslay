package net.kibotu.sandbox.unity.android.network;

import com.koushikdutta.async.http.socketio.Acknowledge;
import com.unity3d.player.UnityPlayer;
import net.kibotu.sandbox.chat.client.android.network.SocketHandler;
import org.json.JSONArray;
import org.json.JSONObject;

/**
 * send message to unity: UnityPlayer.UnitySendMessage("GameObjectName1", "MethodName1", "Message to send");
 */
public class AndroidUnitySocketHandlerProxy implements SocketHandler {

    private static final String TAG = AndroidUnitySocketHandlerProxy.class.getSimpleName();

    public AndroidUnitySocketHandlerProxy() {
    }

    @Override
    public void EventCallback(final JSONArray argument, final Acknowledge acknowledge) {
        UnityPlayer.UnitySendMessage("AndroidSocketClient", "StringCallback", argument == null ? "no argument" : argument.toString());
    }

    @Override
    public void StringCallback(final String message, final Acknowledge acknowledge) {
        UnityPlayer.UnitySendMessage("AndroidSocketClient", "StringCallback", message);
    }

    @Override
    public void JSONCallback(final JSONObject jsonObject, final Acknowledge acknowledge) {
        UnityPlayer.UnitySendMessage("AndroidSocketClient", "JSONCallback", jsonObject == null ? "{}" : jsonObject.toString());
    }

    @Override
    public void DisconnectCallback(final Exception e) {
        UnityPlayer.UnitySendMessage("AndroidSocketClient", "DisconnectCallback", e == null ? "no error" : e.getMessage());
    }

    @Override
    public void ErrorCallback(final String error) {
        UnityPlayer.UnitySendMessage("AndroidSocketClient", "ErrorCallback", error);
    }

    @Override
    public void ReconnectCallback() {
        UnityPlayer.UnitySendMessage("AndroidSocketClient", "ReconnectCallback", null);
    }
}
