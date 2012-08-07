using System;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Edrive.App_Start.MVCToolsStart), "PreStart")]

namespace Edrive.App_Start {
    public static class MVCToolsStart {
        public static void PreStart() {
            // Add your start logic here
        }
    }
}