export const formatDate = (date?:string): string => {
    if (!date) {
        return "";
    }
    const dateTimeParts = date.split("T")[0];
    const dateParts = dateTimeParts.split("-");
    return `${dateParts[0]}-${dateParts[1].padStart(2, "0")}-${dateParts[2].padStart(2, "0")}`;
};