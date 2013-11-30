package net.kibotu.sandbox.chat.client.android.logging;

import android.app.Activity;
import android.util.Log;
import android.widget.Toast;

import static net.kibotu.sandbox.chat.client.android.logging.Logger.Level.*;

/**
 * Concrete Android Logger
 *
 * @author <a href="mailto:jan.rabe@wooga.net">Jan Rabe</a>
 */
public class LogcatLogger implements ILogger {

    public LogcatLogger () {
    }

    @Override
    public void debug ( final String tag, final String message ) {
        Log.d( tag + DEBUG.TAG, message );
    }

    @Override
    public void verbose ( final String tag, final String message ) {
        Log.d( tag + VERBOSE.TAG, message );
    }

    @Override
    public void information ( final String tag, final String message ) {
        Log.d( tag + INFO.TAG, message );
    }

    @Override
    public void warning ( final String tag, final String message ) {
        Log.d( tag + WARNING.TAG, message );
    }

    @Override
    public void error ( final String tag, final String message ) {
        Log.d( tag + ERROR.TAG, message );
    }

	@Override
	public void toast(final Object context, final String message) {
		final Activity activity = ((Activity)context);
		activity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				Toast.makeText(activity, message, Toast.LENGTH_SHORT).show();
			}
		});		
	}
}