using System;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.MVCToolsStart), "PreStart")]

namespace $rootnamespace$.App_Start {
    public static class MVCToolsStart {
        public static void PreStart() {
            // Add your start logic here
        }
    }
}