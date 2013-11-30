package net.kibotu.sandbox.chat.client.android;

import android.app.Activity;
import android.os.Bundle;
import android.text.method.ScrollingMovementMethod;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;
import com.koushikdutta.async.AsyncServer;
import net.kibotu.sandbox.chat.client.android.logging.LogcatLogger;
import net.kibotu.sandbox.chat.client.android.logging.Logger;
import net.kibotu.sandbox.chat.client.android.network.AndroidSocketHandler;
import net.kibotu.sandbox.chat.client.android.network.SocketClient;
import net.kibotu.sandbox.chat.client.android.network.UdpSocketClient;
import org.json.JSONObject;

import static net.kibotu.sandbox.chat.client.android.logging.Logger.Level.DEBUG;

public class ChatClient extends Activity {

    public static final String TAG = ChatClient.class.getSimpleName();
    private static Activity context;
    private static TextView view;
    public static String uid;
    private String ip;
    private int tcpPort;
    private int udpPort;

    public static void appendText(final String text) {
        context.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                view.append(text);
                view.append("\n");
            }
        });
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        context = this;
        setContentView(R.layout.main);

        Logger.init(new LogcatLogger(), TAG, DEBUG);
        SocketClient.init(new AndroidSocketHandler());
        UdpSocketClient.init(new AndroidSocketHandler());
;
        //ip = "192.168.2.101";
        //ip = "178.0.89.213";
        ip = "172.16.2.141";
        //ip = "172.19.253.37";
        tcpPort = 1337;
        udpPort = 1338;
        uid = "android";

        view = (TextView) findViewById(R.id.output);
        view.setMovementMethod(new ScrollingMovementMethod());

        findViewById(R.id.connect).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(final View v) {
                SocketClient.connect(ip, tcpPort);
                UdpSocketClient.connect(ip, udpPort);
            }
        });

        findViewById(R.id.reconnect).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(final View v) {
                SocketClient.reconnect();
                UdpSocketClient.reconnect();
            }
        });

        findViewById(R.id.disconnect).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(final View v) {
                SocketClient.disconnect();
                UdpSocketClient.disconnect();
            }
        });

        findViewById(R.id.send).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(final View v) {
                JSONObject getJsonObject = SocketClient.getJsonObject(ChatClient.uid, ((EditText) findViewById(R.id.editText)).getText().toString());
                SocketClient.Emit("send", getJsonObject);
                UdpSocketClient.Emit("send", getJsonObject);
                //UdpSocketClient.sendData("ping".getBytes());
            }
        });
    }

    @Override
    public void onDestroy() {
        AsyncServer.getDefault().stop();
        super.onDestroy();
    }
}
