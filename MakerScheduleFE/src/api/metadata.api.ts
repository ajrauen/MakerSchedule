import { sendAxiosRequest } from "@ms/api/api";
import { type ApplicaitonMetadata } from "@ms/types/application-metadata.types";

const BASE_METADATA_ENDPOINT = "api/metadata";

const getApplicaitonMetadata = async () => {
  const req = {
    method: "Get",
    url: BASE_METADATA_ENDPOINT,
  };

  return await sendAxiosRequest<ApplicaitonMetadata>(req);
};

export { getApplicaitonMetadata };
