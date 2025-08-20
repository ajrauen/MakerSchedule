import type { DomainUser } from "@ms/types/domain-user.types";
import type { EventTag } from "@ms/types/event-tags.types";
import type { EventOffering } from "@ms/types/event.types";
import type { Occurrence } from "@ms/types/occurrence.types";
import { createSlice, type PayloadAction } from "@reduxjs/toolkit";

interface AdminState {
  selectedEvent?: EventOffering;
  selectedEventOccurrence?: Occurrence;
  selectedUser?: DomainUser;
  selectedEventTag?: EventTag;
  adminDrawerOpen: boolean;
}

const initialState: AdminState = {
  selectedEvent: undefined,
  selectedEventOccurrence: undefined,
  selectedUser: undefined,
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
    setSelectedUser: (state, action: PayloadAction<DomainUser | undefined>) => {
      state.selectedUser = action.payload;
    },
    setSelectedEventTag: (
      state,
      action: PayloadAction<EventTag | undefined>
    ) => {
      state.selectedEventTag = action.payload;
    },
    setAdminDrawerOpen: (state, action: PayloadAction<boolean>) => {
      state.adminDrawerOpen = action.payload;
    },
  },
});

export const {
  setSelectedEvent,
  setSelectedEventOccurrence,
  setSelectedUser,
  setAdminDrawerOpen,
  setSelectedEventTag,
} = adminSlice.actions;

export const selectAdminState = (state: { admin: AdminState }) => state.admin;

export default adminSlice.reducer;
