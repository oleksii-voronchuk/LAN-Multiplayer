package com.olvor.WifiPlugin;

import android.Manifest;
import android.net.wifi.WifiConfiguration;
import android.net.wifi.WifiManager;
import android.content.Context; 
import android.app.Activity;

public class WifiPlugin
{	
	private Context context;

	public WifiPlugin(Context context)
	{
		this.context = context;
	}

	public void TurnOn()
	{
		WifiManager wifiManager = (WifiManager)context.getSystemService(Context.WIFI_SERVICE);
		if (!wifiManager.isWifiEnabled())
			wifiManager.setWifiEnabled(true);
	}  	
	
	public void Connect(String ssid, String password)
	{
		WifiConfiguration wifiConfig = new WifiConfiguration();
		wifiConfig.SSID = String.format("\"%s\"", ssid);
		wifiConfig.preSharedKey = String.format("\"%s\"", password);

		WifiManager wifiManager = (WifiManager)context.getSystemService(Context.WIFI_SERVICE);
		int netId = wifiManager.addNetwork(wifiConfig);
		wifiManager.disconnect();
		wifiManager.enableNetwork(netId, true);
		wifiManager.reconnect();
	}  
}