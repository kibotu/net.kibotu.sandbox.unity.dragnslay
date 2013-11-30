package net.kibotu.sandbox.chat.client.android.logging;


/**
 * <p>
 * Static logging facet for concrete device specific logger. Depending on
 * Logging level it shows only higher prioritized logs. For instance: if the
 * Logging Level is set to info, no debug messages will be shown, etc.
 * </p>
 * <p>
 * Initialize with <code>Logger.init()</code>
 * </p>
 *
 * @author <a href="mailto:jan.rabe@wooga.net">Jan Rabe</a>
 */
final public class Logger {

	/**
	 * Separator between tags.
	 */
	public static final String SEPARATOR = ".";
	/**
	 * Singleton.
	 */
	private static Logger INSTANCE;
	/**
	 * Logging level.
	 */
	private static Level logLevel;
	/**
	 * Concrete Logger.
	 */
	private static ILogger logger;
	/**
	 * Application Tag.
	 */
	private static String tag;

	/**
	 * Constructor.
	 *
	 * @param logger	- Concrete Logger.
	 * @param tag		- Application tag, gets added to the beginning.
	 * @param level		- Logging level.
	 */
	private Logger(final ILogger logger, final String tag, Level level) {
		Logger.logger = logger;
		Logger.tag = tag + SEPARATOR;
		Logger.logLevel = level;
	}

	/**
	 * Initializing logger.
	 *
	 * @param logger 	- Concrete logger.
	 */
	synchronized public static void init(final ILogger logger) {
		if (INSTANCE == null)
			INSTANCE = new Logger(logger, "Log", Level.DEBUG);
	}

	/**
	 * Initializing logger.
	 *
	 * @param logger	- Concrete Logger.
	 * @param tag	  	- Application tag, gets added to the beginning.
	 * @param level		- Logging level.
	 */
	synchronized public static void init(final ILogger logger, String tag,
			Level level) {
		if (INSTANCE == null)
			INSTANCE = new Logger(logger, tag, level);
	}

	/**
	 * Nullifies the logger instance.
	 */
	synchronized public static void release() {
		INSTANCE = null;
		Logger.logger = null;
		Logger.tag = null;
		Logger.logLevel = null;
	}

	/**
	 * Checks against logging level.
	 *
	 * @param level	- Defined logging level.
	 *
	 * @return true if logging is allowed.
	 */
	private static boolean allowLogging(final Level level) {
		if (logger == null)
			throw new IllegalStateException("'logger' must not be null. (Not initiated?)");
		return logLevel.compareTo(level) <= 0;
	}

	/**
	 * Representing Debug-Logging level.
	 *
	 * @param message	- Actual logging message.
	 */
	public static void d(final String loggingTag, final String message) {
		if (allowLogging(Level.DEBUG))
			logger.debug(tag + loggingTag + SEPARATOR, message);
	}

	/**
	 * Representing Error-Logging level.
	 *
	 * @param message	- Actual logging message.
	 */
	public static void e(final String loggingTag, final String message) {
		if (allowLogging(Level.ERROR))
			logger.error(tag + loggingTag + SEPARATOR, message);
	}

	/**
	 * Gets Logging level.
	 *
	 * @return Currently set logging level.
	 */

	public static Level getLogLevel() {
		return logLevel;
	}

	/**
	 * Sets new Logging level.
	 *
	 * @param logLevel -	new logging level.
	 */
	public static void setLogLevel(final Level logLevel) {
		Logger.logLevel = logLevel;
	}

	/**
	 * Representing Information-Logging level.
	 *
	 * @param message	- Actual logging message.
	 */
	public static void i(final String loggingTag, final String message) {
		if (allowLogging(Level.INFO))
			logger.information(tag + loggingTag + SEPARATOR, message);
	}

	/**
	 * Representing Verbose-Logging level.
	 *
	 * @param message	- Actual logging message.
	 */
	public static void v(final String loggingTag, final String message) {
		if (allowLogging(Level.VERBOSE))
			logger.verbose(tag + loggingTag + SEPARATOR, message);
	}

	/**
	 * Representing Warning-Logging level.
	 *
	 * @param message	- Actual logging message.
	 */
	public static void w(final String loggingTag, final String message) {
		if (allowLogging(Level.WARNING))
			logger.warning(tag + loggingTag + SEPARATOR, message);
	}

	/**
	 * Gets current application tag.
	 *
	 * @return current set prefix tag.
	 */

	public static String getTag() {
		return tag.substring(0, tag.length() - 1);
	}

	/**
	 * Sets new prefix tag.
	 *
	 * @param tag 	- Added to the beginning of all logs.
	 */
	public static void setTag(final String tag) {
		Logger.tag = tag + SEPARATOR;
	}

	/**
	 * Alert message.
	 *
	 * @param context	- Applciation Context
	 * @param message	- Alert Message.
	 */
	public static void toast(final Object context, final String message) {
		if (allowLogging(Level.DEBUG))
			Logger.toast(context, message);
	}

	/**
	 * Represents the logging levels.
	 */
	public static enum Level {
		DEBUG("D"), VERBOSE("V"), INFO("I"), WARNING("W"), ERROR("E"), NO_LOGGING("");
		public final String TAG;
		private Level(final String tag) {
			TAG = tag;
		}
	}
}