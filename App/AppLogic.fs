namespace WindowsPhoneApp

open System
open System.Net
open System.Windows
open System.Windows.Controls
open System.Windows.Documents
open System.Windows.Ink
open System.Windows.Input
open System.Windows.Media
open System.Windows.Media.Animation
open System.Windows.Shapes
open System.Windows.Navigation
open Microsoft.Phone.Controls
open Microsoft.Phone.Shell

#nowarn "40"
[<AutoOpen>]
module private Utilities = 
                                                                                      
    let utilityData = [ (1, "one"); (2, "two") ]

    let utilityFunction x y = 
        x + y * 2.0

    /// This is an implementation of the dynamic lookup operator for binding
    /// Xaml objects by name.
    let (?) (source:obj) (s:string) =
        match source with 
        | :? ResourceDictionary as r ->  r.[s] :?> 'T
        | :? Control as source -> 
            match source.FindName(s) with 
            | null -> invalidOp (sprintf "dynamic lookup of Xaml component %s failed" s)
            | :? 'T as x -> x
            | _ -> invalidOp (sprintf "dynamic lookup of Xaml component %s failed because the component found was of type %A instead of type %A"  s (s.GetType()) typeof<'T>)
        | _ -> invalidOp (sprintf "dynamic lookup of Xaml component %s failed because the source object was of type %A. It must be a control or a resource dictionary" s (source.GetType()))

type Class1() = 
    member x.Property = "property"


/// This type implements the main page of the application
type MainPage() as this =
    inherit PhoneApplicationPage()

    // Load the Xaml for the page.
    do Application.LoadComponent(this, new System.Uri("/WindowsPhoneApp;component/MainPage.xaml", System.UriKind.Relative))

    // Bind named Xaml components relevant to this page.
    let appBarButton1 : ApplicationBarIconButton = this.ApplicationBar.Buttons.[0] |> unbox 
    let appBarButton2 : ApplicationBarIconButton = this.ApplicationBar.Buttons.[1] |> unbox  
    let results : TextBlock = this?Results

    // Set the behaviour of the buttons
    do appBarButton1.Click.Add(fun _ -> results.Text <- "Yes, I'm F#")
    do appBarButton2.Click.Add(fun _ -> results.Text <- "F# rocks!")
    

/// One instance of this type is created in the application host project.
type App(app:Application) = 
    // Global handler for uncaught exceptions. 
    // Note that exceptions thrown by ApplicationBarItem.Click will not get caught here.
    do app.UnhandledException.Add(fun e -> 
            if (System.Diagnostics.Debugger.IsAttached) then
                // An unhandled exception has occurred, break into the debugger
                System.Diagnostics.Debugger.Break();
     )

    let rootFrame = new PhoneApplicationFrame();

    do app.RootVisual <- rootFrame;

    // Handle navigation failures
    do rootFrame.NavigationFailed.Add(fun _ -> 
        if (System.Diagnostics.Debugger.IsAttached) then
            // A navigation has failed; break into the debugger
            System.Diagnostics.Debugger.Break())

    // Navigate to the main page 
    do rootFrame.Navigate(new Uri("/WindowsPhoneApp;component/MainPage.xaml", UriKind.Relative)) |> ignore

    // Required object that handles lifetime events for the application
    let service = PhoneApplicationService()
    // Code to execute when the application is launching (eg, from Start)
    // This code will not execute when the application is reactivated
    do service.Launching.Add(fun _ -> ())
    // Code to execute when the application is closing (eg, user hit Back)
    // This code will not execute when the application is deactivated
    do service.Closing.Add(fun _ -> ())
    // Code to execute when the application is activated (brought to foreground)
    // This code will not execute when the application is first launched
    do service.Activated.Add(fun _ -> ())
    // Code to execute when the application is deactivated (sent to background)
    // This code will not execute when the application is closing
    do service.Deactivated.Add(fun _ -> ())

    do app.ApplicationLifetimeObjects.Add(service) |> ignore
