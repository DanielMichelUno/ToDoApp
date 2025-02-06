import { useEffect, useState } from "react";
import { SelectOption, ToDoTask } from "../../interfaces";
import { useToDoAppi } from "../../hooks/useToDoAppi";
import { formatDate, udpateInput, Verbs } from "../../utils";
import { apiRoot, apiRoutes, ToDoStatuses } from "../../definitions";

interface UpdateTaskFormProps {
    todoTask: ToDoTask;
    onDismiss: (shouldRefresh: boolean) => void;
}
export const UpdateTaskForm = ({ todoTask: originalTask, onDismiss }: UpdateTaskFormProps) => {
    const [localTask, setLocalTask] = useState<ToDoTask>(originalTask);
    const [getStatuses] = useToDoAppi(Verbs.Get, apiRoot, apiRoutes.toDo, apiRoutes.statuses);
    const [updateTask, isUpdating] = useToDoAppi(Verbs.Put, apiRoot, apiRoutes.toDo);
    const [error, setError] = useState<string>('');

    const [statusOption, setStatusOption] = useState<SelectOption<ToDoStatuses>[]>([]);

    useEffect(() => {
        getStatuses().then((data) => setStatusOption(data));
    }, []);

    const isValid = localTask.description.length > 0 && localTask.statusId > 0;

    const onUpdateLocalTaskInput = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => udpateInput(e, localTask, setLocalTask);

    const onSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await updateTask({ body: {Id:localTask.id, StatusId: localTask.statusId, Description:localTask.description, DueDate: localTask.dueDate} });
            onDismiss(true);
        }
        catch (e) {
            console.log(e);
            setError('Failed to update task');
        }
    }

    return (
        isUpdating ? <p>Updating...</p> :
            <div>
                <h1>Update Task</h1>
                {error && <p>{error}</p>}
                <form onSubmit={onSubmit}>
                    <label>
                        Description:
                        <input
                            type="text"
                            value={localTask.description}
                            name="description"
                            onChange={onUpdateLocalTaskInput}
                        />
                    </label>
                    <label>
                        Status:
                        <select value={localTask.statusId} name='statusId' onChange={onUpdateLocalTaskInput}>
                            {statusOption.map((status: SelectOption<ToDoStatuses>) => (
                                <option
                                    key={`status-option-${status.key}`}
                                    value={status.key}>
                                    {status.text}
                                </option>
                            ))}
                        </select>
                    </label>
                    <label>
                        Due Date:
                        <input name='dueDate' type="date" value={formatDate(localTask.dueDate)} onChange={onUpdateLocalTaskInput} />
                    </label>
                    <button type="submit" disabled={!isValid}>Update</button>
                    <button type="button" onClick={() => onDismiss(false)}>Cancel</button>
                </form>
            </div>
    )
}