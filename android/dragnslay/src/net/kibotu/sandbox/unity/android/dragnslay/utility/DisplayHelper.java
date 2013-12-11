package net.kibotu.sandbox.unity.android.dragnslay.utility;

import android.graphics.Point;
import net.kibotu.sandbox.unity.android.dragnslay.UnityPlayerNativeActivity;

public class DisplayHelper {

    private DisplayHelper() {
    }

    public static int getDisplayWidth() {
        return getSize().x;
    }

    public static int getDisplayHeight() {
        return getSize().y;
    }

    public static Point getSize() {
        Point ret = new Point();
        UnityPlayerNativeActivity.context.getWindowManager().getDefaultDisplay().getSize(ret);
        return ret;
    }
}
