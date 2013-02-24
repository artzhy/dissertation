package uk.ac.ed.inf.mandelbrotmaps;

import android.util.Log;


class RenderThread extends Thread {
	private AbstractFractalView mjCanvas;
	private volatile boolean abortThisRendering = false;
	public boolean isRunning = false;
	private int threadID = -1;
	
	public RenderThread(AbstractFractalView mjCanvasHandle, int _threadID, int _noOfThreads) {
		mjCanvas = mjCanvasHandle;
		threadID = _threadID;
		//setPriority(Thread.MAX_PRIORITY);
	}
	
	public void abortRendering() {
		abortThisRendering = true;
	}
	
	public void allowRendering() {
		abortThisRendering = false;
	}
	
	public boolean abortSignalled() {
		return abortThisRendering;
	}
	
	public void run() {
		while(true) {
			try {
				Rendering newRendering = mjCanvas.getNextRendering(threadID);
				Log.d("TEST", "HERE");
				
				mjCanvas.computeAllPixels(newRendering.getPixelBlockSize(), threadID);
				// TODO: wait until result is back...
				
				
				abortThisRendering = false;
			} catch (InterruptedException e) {
				return;
			}
		}
	}
}