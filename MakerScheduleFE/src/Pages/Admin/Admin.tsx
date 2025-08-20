import { TabPanel } from "@ms/Components/LayoutComponents/TabPanel/TabPanel";
import { AdminEvents } from "@ms/Pages/Admin/Events/Events";
import { AdminEventTags } from "@ms/Pages/Admin/EventTags/EventTags";
import { AdminUsers } from "@ms/Pages/Admin/User/User";
import { useAppDispatch } from "@ms/redux/hooks";
import {
  setAdminDrawerOpen,
  setSelectedEvent,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import { Tab, Tabs } from "@mui/material";
import { useState } from "react";

const Admin = () => {
  const [value, setValue] = useState(0);
  const dispatch = useAppDispatch();

  const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
    resetState();
    setValue(newValue);
  };

  const resetState = () => {
    dispatch(setSelectedEvent(undefined));
    dispatch(setSelectedEventOccurrence(undefined));
    dispatch(setAdminDrawerOpen(false));
  };

  function a11yProps(index: number) {
    return {
      id: `vertical-tab-${index}`,
      "aria-controls": `vertical-tabpanel-${index}`,
    };
  }

  return (
    <div className="flex h-full w-full">
      <Tabs
        orientation="vertical"
        variant="scrollable"
        value={value}
        onChange={handleChange}
        aria-label="Vertical tabs example"
      >
        <Tab label="Events" {...a11yProps(0)} />
        <Tab label="Users" {...a11yProps(1)} />
        <Tab label="Event Types" {...a11yProps(2)} />
      </Tabs>
      <TabPanel index={0} value={value}>
        <AdminEvents />
      </TabPanel>
      <TabPanel index={1} value={value}>
        <AdminUsers />
      </TabPanel>
      <TabPanel index={2} value={value}>
        <AdminEventTags />
      </TabPanel>
    </div>
  );
};

export { Admin };
