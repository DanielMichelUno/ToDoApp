import { useEffect, useState } from "react";
import { apiRoot, apiRoutes, ToDoStatuses } from "../../definitions";
import { useToDoAppi } from "../../hooks/useToDoAppi";
import { formatDate, udpateInput, Verbs } from "../../utils";
import { SelectOption, ToDoTask } from "../../interfaces";

interface CreateTaskFormProps {
    onDismiss: (shouldRefresh: boolean) => void;
}

export const CreateTaskForm = ({ onDismiss }: CreateTaskFormProps) => {
    const [createTask, isLoading] = useToDoAppi(Verbs.Post, apiRoot, apiRoutes.toDo);
    const [localTask, setLocalTask] = useState<ToDoTask>({ description: '', statusId: ToDoStatuses.ToDo });
    const [getStatuses] = useToDoAppi(Verbs.Get, apiRoot, apiRoutes.toDo, apiRoutes.statuses);
    const [error, setError] = useState<string>('');
    const [statusOption, setStatusOption] = useState<SelectOption<ToDoStatuses>[]>([]);

    const onUpdateLocalTaskInput = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => udpateInput(e, localTask, setLocalTask);
    
    useEffect(() => {
        getStatuses().then((data) => setStatusOption(data));
    }, []);

    const onSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await createTask({ body: {Id:localTask.id, StatusId: localTask.statusId, Description:localTask.description, DueDate: localTask.dueDate} });
            onDismiss(true);
        }
        catch (e) {
            console.log(e);
            setError('Failed to update task');
        }
    }

    const isValid = localTask.description.length > 0 && localTask.statusId > 0;

    return (
        isLoading ? <p>Updating...</p> :
            <div>
                <h1>Create Task</h1>
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
                    <button type="submit" disabled={!isValid}>Create</button>
                    <button type="button" onClick={() => onDismiss(false)}>Cancel</button>
                </form>
            </div>
    );
}