# StravaFix

I recently noticed that Strava doesn't really do a smart job about when you pause the run, or forget to stop the run at the end.  So I made a small C# utility.  The link to the executable is here.  https://github.com/rdejournett/StravaFix/blob/master/StravaFix.exe

And the full repo source is here. https://github.com/rdejournett/StravaFix

Basically you first go to Strava and download the GFX file.  Go to the Strava website, pull up your activity, click the wrench icon (middle left of screen) and select Export GPX.

Start the utility now.  Then drag and drop that Strava file to the utility, and click the button on the utility.

There are two checkboxes.  
1)  I Peed - ie look for a pause point in the run, and fix the times beyond that.
2)  I stopped - ie look at the end of the run, and look to where your HR has gone down.  It will delete timepoints after that.  Ie if your HR dropped from 180 to 140 it will mark that as a stop point.  It measures every 30 seconds, so you'll be able to get within 30 sec of your finish time.

Then just upload the file back to strava (delete the old run first).  Deleting a run is  bit of a trick, you go to the website, and find your activity, then click the wrench icon on the middle-left hand side, and "Delete".  To add the fixed run, look for the orange + sign on the top right of the screen, and click Upload activity.

Let me know if you like it and want to use it.
