import { Button } from "@mui/material";

interface FormFooterProps {
  onCancel: () => void;
  onSubmit?: () => void;
  onDelete?: () => void;
  areActionsDisabled?: boolean;
  isLoading?: boolean;
  isDeleting?: boolean;
  showDelete?: boolean;
  cancelButtonText?: string;
  saveButtonText?: string;
}

const FormFooter = ({
  onCancel,
  onSubmit,
  onDelete,
  areActionsDisabled,
  isLoading,
  isDeleting,
  showDelete = false,
  cancelButtonText = "Cancel",
  saveButtonText = "Save",
}: FormFooterProps) => {
  return (
    <div className="pt-4  flex">
      {showDelete && (
        <Button
          color="error"
          type="button"
          onClick={onDelete}
          loading={isDeleting}
        >
          Delete
        </Button>
      )}
      <div className=" ml-auto gap-3 flex">
        <Button type="button" onClick={onCancel} disabled={areActionsDisabled}>
          {cancelButtonText}
        </Button>
        <Button
          type="submit"
          variant="contained"
          disabled={areActionsDisabled}
          loading={isLoading}
          onClick={onSubmit}
        >
          {saveButtonText}
        </Button>
      </div>
    </div>
  );
};

export { FormFooter };
