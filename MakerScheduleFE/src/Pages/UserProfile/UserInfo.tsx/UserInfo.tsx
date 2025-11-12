import { zodResolver } from "@hookform/resolvers/zod";
import { updateUserProfile } from "@ms/api/domain-user.api";
import { updateUserInfoValidationSchema } from "@ms/common/FormSchemas/UserInfo.schema";
import { Accordion } from "@ms/Components/Accordion/Accordion";
import { UserForm } from "@ms/Components/UserForm/UserForm";
import type {
  DomainUser,
  UpdateDomainUserRequest,
} from "@ms/types/domain-user.types";
import { useMutation } from "@tanstack/react-query";
import { useEffect } from "react";
import { FormProvider, useForm } from "react-hook-form";

interface UserInfoProps {
  userData?: DomainUser;
}

const UserInfo = ({ userData }: UserInfoProps) => {
  const registerInitialFormData: UpdateDomainUserRequest = {};

  const { reset, handleSubmit, ...rest } = useForm<UpdateDomainUserRequest>({
    resolver: zodResolver(updateUserInfoValidationSchema),
    defaultValues: registerInitialFormData,
  });

  useEffect(() => {
    if (userData) {
      reset(userData);
    }
  }, [userData, reset]);

  const { mutate: doUpdateUserProfile } = useMutation({
    mutationKey: ["updateUserInfo"],
    mutationFn: updateUserProfile,
    meta: {
      successMessage: "Profile Updated",
      errorMessage: "Failed to update profile",
    },
  });

  const submit = (data: UpdateDomainUserRequest) => {
    if (!userData?.id) return;

    doUpdateUserProfile({ userId: userData.id, data });

    reset(undefined, { keepValues: true });
  };

  return (
    <Accordion
      title="User Profile"
      containerClassName="w-full max-w-2xl"
      defaultExpanded
      onSubmit={handleSubmit(submit)}
    >
      <div className="flex flex-col gap-4 w-full">
        <FormProvider {...rest} handleSubmit={handleSubmit} reset={reset}>
          <UserForm includePassword={false} />
        </FormProvider>
      </div>
    </Accordion>
  );
};

export { UserInfo };
