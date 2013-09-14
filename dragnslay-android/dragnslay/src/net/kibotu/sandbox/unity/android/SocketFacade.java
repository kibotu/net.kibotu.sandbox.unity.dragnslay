package net.kibotu.sandbox.unity.android;

import android.util.Log;
import org.jetbrains.annotations.NotNull;

public class SocketFacade {

    public static void Emit(@NotNull String name, @NotNull String message) {
        Log.v(name, message);
    }
}
