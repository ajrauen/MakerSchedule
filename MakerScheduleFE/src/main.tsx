import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";

import { QueryClientProvider } from "@tanstack/react-query";
import { queryClient } from "@ms/common/query-client";
import { App } from "@ms/App";
import ErrorBoundary from "@ms/common/ErrorBoundary";
import { ToastContainer } from "react-toastify";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <ErrorBoundary>
      <ToastContainer position="bottom-right" />
      <QueryClientProvider client={queryClient}>
        <App />
      </QueryClientProvider>
    </ErrorBoundary>
  </StrictMode>
);
