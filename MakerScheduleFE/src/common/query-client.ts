import { MutationCache, QueryCache, QueryClient } from "@tanstack/react-query";
import { toast } from "react-toastify";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
      retry: 1,
      throwOnError: true,
    },
    mutations: {
      retry: 1,
    },
  },
  queryCache: new QueryCache({
    onError: (error, query) => {
      if (query?.meta?.errorMessage) {
        toast.error(query.meta.errorMessage as string);
      }
    },
    onSuccess: (data, query) => {
      if (query?.meta?.successMessage) {
        toast.success(query.meta.successMessage as string);
      }
    },
  }),

  mutationCache: new MutationCache({
    onError: (error, variables, context, mutation) => {
      if (mutation?.meta?.errorMessage) {
        toast.error(mutation.meta.errorMessage as string);
      }
    },
    onSuccess: (data, variables, context, mutation) => {
      if (mutation?.meta?.successMessage) {
        toast.success(mutation.meta.successMessage as string);
      }
    },
  }),
});

export { queryClient };
