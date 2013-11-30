package net.kibotu.sandbox.network;

public interface AsyncTaskCallback<T> {
    public void callback(T... params);
}