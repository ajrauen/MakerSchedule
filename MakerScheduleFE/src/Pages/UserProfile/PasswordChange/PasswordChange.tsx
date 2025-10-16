import { zodResolver } from "@hookform/resolvers/zod";
import { updateUserPassword } from "@ms/api/domain-user.api";
import { updateUserPasswordValidationSchema } from "@ms/common/FormSchemas/UserInfo.schema";
import { Accordion } from "@ms/Components/Accordion/Accordion";
import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import type {
  DomainUser,
  UpdateUserPassword,
} from "@ms/types/domain-user.types";
import { useMutation } from "@tanstack/react-query";
import { useForm } from "react-hook-form";

const initialChangeUserPasswordData: UpdateUserPassword = {
  currentPassword: "",
  newPassword: "",
  confirmPassword: "",
};

interface PasswordChangeProps {
  userData?: DomainUser;
}

const PasswordChange = ({ userData }: PasswordChangeProps) => {
  const { control, handleSubmit } = useForm<UpdateUserPassword>({
    resolver: zodResolver(updateUserPasswordValidationSchema),
    defaultValues: initialChangeUserPasswordData,
  });

  const { mutate: doUpdateUserPassword } = useMutation({
    mutationKey: ["updateUserPassword"],
    mutationFn: updateUserPassword,
    meta: {
      successMessage: "Password Updated",
      errorMessage: "Failed to update password",
    },
  });

  const onSubmit = (data: UpdateUserPassword) => {
    if (!userData?.id) return;
    doUpdateUserPassword({
      userId: userData.id,
      currentPassword: data.currentPassword,
      newPassword: data.newPassword,
    });
  };

  return (
    <Accordion
      title="Change Password"
      containerClassName="w-full max-w-2xl"
      defaultExpanded
      onSubmit={handleSubmit(onSubmit)}
    >
      <div className="flex flex-col gap-4 w-full">
        <FormTextField
          label="Current Password"
          control={control}
          name="currentPassword"
          type="password"
        />
        <FormTextField
          label="New Password"
          control={control}
          name="newPassword"
          type="password"
        />
        <FormTextField
          label="Confirm Password"
          control={control}
          name="confirmPassword"
          type="password"
        />
      </div>
    </Accordion>
  );
};

export { PasswordChange };
