import React from "react";

interface ErrorBoundaryProps {
  children: React.ReactNode;
}

interface ErrorBoundaryState {
  hasError: boolean;
  error?: Error;
}

class ErrorBoundary extends React.Component<
  ErrorBoundaryProps,
  ErrorBoundaryState
> {
  constructor(props: ErrorBoundaryProps) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError(error: Error) {
    return { hasError: true, error };
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
    console.error("ErrorBoundary caught an error", error, errorInfo);
  }

  handleReload = () => {
    window.location.reload();
  };

  render() {
    if (this.state.hasError) {
      return (
        <div className="flex flex-col items-center justify-center h-full p-8">
          <h2 className="text-2xl font-bold mb-4">
            🧑‍💻 Oops! Our code tripped over its own shoelaces.
          </h2>
          <p className="mb-2 text-lg">
            Don't worry, even the best apps have clumsy days.
          </p>
          <pre className="mb-4 text-red-600">{this.state.error?.message}</pre>
          <button
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition"
            onClick={this.handleReload}
          >
            Try turning it off and on again
          </button>
        </div>
      );
    }
    return this.props.children;
  }
}

export default ErrorBoundary;
