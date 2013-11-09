package net.kibotu.sandbox.unity.android.network;

import android.util.Log;
import com.koushikdutta.async.http.socketio.Acknowledge;
import com.unity3d.player.UnityPlayer;
import net.kibotu.sandbox.chat.client.android.network.SocketHandler;
import org.json.JSONArray;
import org.json.JSONObject;

/**
 * send message to unity: UnityPlayer.UnitySendMessage("GameObjectName1", "MethodName1", "Message to send");
 */
public class AndroidUnitySocketHandler extends SocketHandler {

    private static final String TAG = AndroidUnitySocketHandler.class.getSimpleName();

    @Override
    protected void EventCallback(final JSONArray argument, final Acknowledge acknowledge) {
        Log.v(TAG, "onEvent 'send' ack = " + acknowledge.toString() + " args = " + argument.toString());
        UnityPlayer.UnitySendMessage("AndroidSocketHandler", "StringCallback", argument.toString());
    }

    @Override
    protected void StringCallback(final String message, final Acknowledge acknowledge) {
        Log.v(TAG, "onString " + message + " " + acknowledge);
        UnityPlayer.UnitySendMessage("AndroidSocketHandler", "StringCallback", message);
    }

    @Override
    protected void JSONCallback(final JSONObject jsonObject, final Acknowledge acknowledge) {
        Log.v(TAG, "onJson " + jsonObject + " " + acknowledge);
        UnityPlayer.UnitySendMessage("AndroidSocketHandler", "JSONCallback", jsonObject.toString());
    }

    @Override
    protected void DisconnectCallback(final Exception e) {
        Log.v(TAG, "onDisconnect " + e.getMessage());
        UnityPlayer.UnitySendMessage("AndroidSocketHandler", "DisconnectCallback", e.getMessage());
    }

    @Override
    protected void ErrorCallback(final String error) {
        Log.v(TAG, "onError " + error);
        UnityPlayer.UnitySendMessage("AndroidSocketHandler", "ErrorCallback", error);
    }

    @Override
    protected void ReconnectCallback() {
        Log.v(TAG, "onReconnect");
        UnityPlayer.UnitySendMessage("AndroidSocketHandler", "ReconnectCallback", null);
    }
}
