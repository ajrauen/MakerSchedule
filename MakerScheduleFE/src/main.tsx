import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import {
  RouterProvider,
  createRouter,
  createRootRoute,
  createRoute,
} from "@tanstack/react-router";
import { Login } from "@ms/Pages/Login/Login.tsx";
import App from "@ms/App";
import { QueryClientProvider } from "@tanstack/react-query";
import { queryClient } from "@ms/common/query-client";
import { Home } from "@ms/Pages/Home/Home";

// Example route components
const About = () => <div>About Page</div>;

// Create the root route
const rootRoute = createRootRoute({
  component: App,
});

// Create child routes
const homeRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: "/",
  component: Home,
});
const aboutRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: "/about",
  component: About,
});

const loginRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: "/login",
  component: Login,
});

// Build the route tree
const routeTree = rootRoute.addChildren([homeRoute, aboutRoute, loginRoute]);

// Create the router
const router = createRouter({
  routeTree,
});

// Register router type for type safety
declare module "@tanstack/react-router" {
  interface Register {
    router: typeof router;
  }
}

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  </StrictMode>
);
