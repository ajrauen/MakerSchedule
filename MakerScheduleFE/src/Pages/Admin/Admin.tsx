import { TabPanel } from "@ms/Components/LayoutComponents/TabPanel/TabPanel";
import { AdminEvents } from "@ms/Pages/Admin/Events/Events";
import { Box, Paper, Tab, Tabs } from "@mui/material";
import { useState } from "react";

const Admin = () => {
  const [value, setValue] = useState(0);

  const handleChange = (event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
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
        <Tab label="Item Two" {...a11yProps(1)} />
      </Tabs>
      <TabPanel index={0} value={value}>
        <AdminEvents />
      </TabPanel>
    </div>
  );
};

export { Admin };
