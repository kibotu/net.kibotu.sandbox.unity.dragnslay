package net.kibotu.sandbox.chat.client.android.network;


import android.os.Handler;
import org.jetbrains.annotations.NotNull;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.net.*;
import java.util.Timer;
import java.util.TimerTask;

public class UdpSocketClient implements Runnable {

    // Default packet size. Note: UDP header provides only
    // 16 bits for data size - which means the maximum data
    // size is 0xffff (unsigned) or 65535.
    private static final int DEFAULT_PACKET_SIZE = 512;
    private static final int MAX_PACKET_SIZE = 65535;
    private static final String TAG = UdpSocketClient.class.getSimpleName();
    private static SocketHandler socketHandler;
    private static DatagramSocket socket;
    private static InetAddress host;
    private static int port;
    private static boolean isConnected;

    public static void init(@NotNull final SocketHandler socketHandler) {
        UdpSocketClient.socketHandler = socketHandler;
    }

    public synchronized static void connect(@NotNull final String host, final int port) {
        if (socket != null && isConnected())
            return;
        if (socketHandler == null)
            throw new IllegalStateException("No SocketHandler defined. Please use init() before.");

        try {
            socket = new DatagramSocket();
            UdpSocketClient.host = InetAddress.getByName(host);
            UdpSocketClient.port = port;
        } catch (UnknownHostException | SocketException e) {
            e.printStackTrace();
        }

        new Thread(new UdpSocketClient()).start();
    }

    public synchronized static void disconnect() {
        if (socket == null || !isConnected()) return;
        socket.disconnect();
        isConnected = true;
    }

    public synchronized static void reconnect() {
        if (socket == null || isConnected()) return;
        // socket.reconnect();
    }

    public static boolean isConnected() {
        return isConnected; // socket.isConnected()
    }

    public static void Emit(@NotNull final String name, @NotNull final JSONArray args) {
        if (socket == null || !isConnected()) return;
        try {
            sendData(new JSONObject().put(name, args).toString().getBytes());
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public static void Emit(@NotNull final String name, @NotNull final JSONObject args) {
        Emit(name, new JSONArray().put(args));
    }

    public static void Emit(@NotNull final String name, @NotNull final String args) {
        try {
            Emit(name, new JSONObject(args));
        } catch (JSONException e) {
            e.printStackTrace();
            socketHandler.ErrorCallback(""+ e.getMessage());
        }
    }

    public static void sendData(final byte[] data) {
        if (socket == null || !isConnected()) return;
        new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    socket.send(new DatagramPacket(data, data.length, host, port));
                } catch (IOException e) {
                    e.printStackTrace();
                    socketHandler.ErrorCallback(""+ e.getMessage());
                }
            }
        }).start();
    }

    public static DatagramSocket socket() {
        return socket;
    }

    public void repeatTask() {
        final Handler handler = new Handler();
        Timer timer = new Timer();
        TimerTask doAsynchronousTask = new TimerTask() {
            @Override
            public void run() {
                handler.post(new Runnable() {
                    public void run() {
                        try {
                            // do timed stuff
                        } catch (Exception e) {
                            e.printStackTrace();
                            socketHandler.ErrorCallback(""+ e.getMessage());
                        }
                    }
                });
            }
        };
        timer.schedule(doAsynchronousTask, 0, 50000);
    }

    @Override
    public void run() {
        isConnected = true;
        socketHandler.ConnectCallback(null, null);   // todo udp has no socket io client or error message to delegate
        while (isConnected()) {
            byte[] data = new byte[DEFAULT_PACKET_SIZE];
            DatagramPacket packet = new DatagramPacket(data, data.length);
            try {
                socket.receive(packet);
                String message = new String(packet.getData()).trim();
                if (message.trim().equalsIgnoreCase("pong")) {
                    sendData("ping".getBytes());
                    socketHandler.EventCallback(new JSONArray().put(new JSONObject().put("message", message)), null);
                } else {
                    try {
                        socketHandler.EventCallback(new JSONArray().put(new JSONObject(message)), null);
                    } catch (JSONException e) {
                        socketHandler.ErrorCallback(""+ e.getMessage());
                        e.printStackTrace();
                    }
                }
            } catch (IOException | JSONException e) {
                e.printStackTrace();
                socketHandler.ErrorCallback(""+ e.getMessage());
            }
        }
    }
}