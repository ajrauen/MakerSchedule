import type { DomainUser } from "@ms/types/domain-user.types";
import type { EventOffering } from "@ms/types/event.types";
import type { Occurrence } from "@ms/types/occurrence.types";
import { createSlice, type PayloadAction } from "@reduxjs/toolkit";

interface AdminState {
  selectedEvent?: EventOffering;
  selectedEventOccurrence?: Occurrence;
  selectedDomainUser?: DomainUser;
  adminDrawerOpen: boolean;
}

const initialState: AdminState = {
  selectedEvent: undefined,
  selectedEventOccurrence: undefined,
  selectedDomainUser: undefined,
  adminDrawerOpen: false,
};

export const adminSlice = createSlice({
  name: "admin",
  initialState,
  reducers: {
    setSelectedEvent: (
      state,
      action: PayloadAction<EventOffering | undefined>
    ) => {
      state.selectedEvent = action.payload;
    },
    setSelectedEventOccurrence: (
      state,
      action: PayloadAction<Occurrence | undefined>
    ) => {
      state.selectedEventOccurrence = action.payload;
    },
    setSelectedDomainUser: (
      state,
      action: PayloadAction<DomainUser | undefined>
    ) => {
      state.selectedDomainUser = action.payload;
    },
    setAdminDrawerOpen: (state, action: PayloadAction<boolean>) => {
      state.adminDrawerOpen = action.payload;
    },
  },
});

export const {
  setSelectedEvent,
  setSelectedEventOccurrence,
  setSelectedDomainUser,
  setAdminDrawerOpen,
} = adminSlice.actions;

export const selectAdminState = (state: { admin: AdminState }) => state.admin;

export default adminSlice.reducer;
