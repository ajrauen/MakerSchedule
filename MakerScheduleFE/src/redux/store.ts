import { configureStore } from "@reduxjs/toolkit";
import adminReducer from "./slices/adminSlice";

const store = configureStore({
  reducer: {
    admin: adminReducer,
  },
});

type RootState = ReturnType<typeof store.getState>;
type AppDispatch = typeof store.dispatch;

export { store, type RootState, type AppDispatch };
