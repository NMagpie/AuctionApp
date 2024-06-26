import { FieldError, UseFormRegister } from "react-hook-form";
import { z, ZodType } from "zod";
import { categoryList } from "../../../common";

export type ManageProductFormData = {
    title: string;
    description: string;
    startTime: Date;
    endTime: Date;
    initialPrice: number;
    category: string;
};

export type ManageProductFormFieldProps = {
    type: string;
    placeholder: string;
    name: ManageProductValidFieldNames;
    register: UseFormRegister<FormData>;
    error: FieldError | undefined;
    valueAsNumber?: boolean;
};

export type ManageProductValidFieldNames =
    | "title"
    | "description"
    | "startTime"
    | "endTime"
    | "initialPrice"
    | "category";

const getCorrectStartDate = () => {
    const date = new Date();

    date.setMinutes(date.getMinutes() + 5);

    return date;
};

const CategorySchema = z.union(
    categoryList.map(c => z.literal(c))
);

export const ManageProductSchema: ZodType<ManageProductFormData> = z
    .object({
        title: z.string().min(1).max(256),
        description: z.string().min(1).max(2048),
        startTime: z.date().min(getCorrectStartDate(), { message: "The Start Time must be greater than current time for at least 5 minutes" }),
        endTime: z.date(),
        initialPrice: z.number({ message: "Enter valid price" }).min(1),
        category: CategorySchema,
    })
    .refine(data => data.endTime > new Date(data.startTime.getTime() + 1 * 60000), {
        message: "The Selling Time must be at least 1 minute long",
        path: ["endTime"],
    });