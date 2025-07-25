import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { FormHelperText } from "@mui/material";

import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";

interface UserFormProps {
  control: any;
  errorCode?: string;
}

const UserForm = ({ control, errorCode }: UserFormProps) => {
  return (
    <>
      <FormTextField label="Email" control={control} name="email" />
      <FormTextField
        label="Password"
        control={control}
        name="password"
        type="password"
      />
      <FormTextField label="First Name" control={control} name="firstName" />
      <FormTextField label="Last Name" control={control} name="lastName" />
      <FormTextField
        label="Phone Number"
        control={control}
        name="phoneNumber"
      />
      <FormTextField label="Address" control={control} name="address" />
      <FormSelect
        label="Preferred Contact Method"
        control={control}
        name="preferredContactMethod"
        options={[
          { label: "Phone", value: "Phone" },
          { label: "Email", value: "Email" },
        ]}
      />

      {errorCode && <FormHelperText error={true}>{errorCode}</FormHelperText>}
    </>
  );
};

export { UserForm };
