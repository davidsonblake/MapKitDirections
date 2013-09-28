using System;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using MonoTouch.MapKit;
using MonoTouch.UIKit;
using System.Drawing;

namespace MapkitDirections
{
    public class MyViewController : UIViewController
    {
        private MKMapView _map;
        private MKMapViewDelegate _mapDelegate;

        public MyViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.Title = "MapKit Sample";

            //Init Map
            _map = new MKMapView
            {
                MapType = MKMapType.Standard,
                ShowsUserLocation = true,
                ZoomEnabled = true,
                ScrollEnabled = true,
                ShowsBuildings = true,
                PitchEnabled = true,

            };

            this.SetToolbarItems(new UIBarButtonItem[] {
    new UIBarButtonItem(UIBarButtonSystemItem.Camera, (s,e) => {
                                                                   
      using (var snapShotOptions = new MKMapSnapshotOptions())
      {
          snapShotOptions.Region = _map.Region;
          snapShotOptions.Scale = UIScreen.MainScreen.Scale;
          snapShotOptions.Size = _map.Frame.Size;
          
          using (var snapShot = new MKMapSnapshotter(snapShotOptions))
          {
              snapShot.Start((snapshot, error) =>
              {
                  if (error == null)
                  {
                      snapshot.Image.SaveToPhotosAlbum(
                          (uiimage, imgError) =>
                              {
                                  if (imgError == null)
                                  {
                                      new UIAlertView("Image Saved", "Map View Image Saved!", null, "OK", null).Show();
                                  }

                               });
                   }
               });
           }
       }
    })
}, false);

            this.NavigationController.ToolbarHidden = false;





            //Create new MapDelegate Instance
            _mapDelegate = new MapDelegate();

            //Add delegate to map
            _map.Delegate = _mapDelegate;

            View = _map;

            //Create Directions
            CreateRoute();
        }

        private void CreateRoute()
        {
            //Create Origin and Dest Place Marks and Map Items to use for directions
            //Start at Xamarin SF Office
            var orignPlaceMark = new MKPlacemark(new CLLocationCoordinate2D(37.797530, -122.402590), null);
            var sourceItem = new MKMapItem(orignPlaceMark);

            //End at Xamarin Cambridge Office
            var destPlaceMark = new MKPlacemark(new CLLocationCoordinate2D(42.374172, -71.120639), null);
            var destItem = new MKMapItem(destPlaceMark);

            //Create Directions request using the source and dest items
            var request = new MKDirectionsRequest
            {
                Source = sourceItem,
                Destination = destItem,
                RequestsAlternateRoutes = true
            };

            var directions = new MKDirections(request);

            //Hit Apple Directions server
            directions.CalculateDirections((response, error) =>
            {
                if (error != null)
                {
                    Console.WriteLine(error.LocalizedDescription);
                }
                else
                {
                    //Add each polyline from route to map as overlay
                    foreach (var route in response.Routes)
                    {
                        _map.AddOverlay(route.Polyline);
                    }
                }
            });
        }

        class MapDelegate : MKMapViewDelegate
        {
            //Override OverLayRenderer to draw Polyline returned from directions
            public override MKOverlayRenderer OverlayRenderer(MKMapView mapView, IMKOverlay overlay)
            {
                if (overlay is MKPolyline)
                {
                    var route = (MKPolyline)overlay;
                    var renderer = new MKPolylineRenderer(route) { StrokeColor = UIColor.Blue };
                    return renderer;
                }
                return null;
            }
        }

    }
}

