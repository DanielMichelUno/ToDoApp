import { useCallback, useState } from "react";
import { buildUrl, Verbs } from "../utils";
import { useLogin } from "./useLogin"

interface FetchParams{
    body?: object;
    param?: string;
}


export const useToDoAppi = (verb: string, ...args: string[]) => {
    const { token } = useLogin();
    const [isLoading, setIsLoading] = useState(false);
    
    const fetchApi = useCallback(async (params?:FetchParams) => {
        try{
            setIsLoading(true);
            if(params?.param) args.push(params.param);
            const route = buildUrl(...args);
            const headers = [['Authorization', `Bearer ${token}`]];
            const needBody = verb === Verbs.Post || verb === Verbs.Put;
            if (params?.body && needBody) {
                headers.push(['Content-Type', 'application/json']);
                headers.push(['Accept', 'application/json']);
            }
            const response = await fetch(route, {
                headers: headers as HeadersInit,
                mode: 'cors',
                method: verb,
                body: needBody && params?.body ? JSON.stringify({...params.body}) : undefined,
            });
            if (!response.ok) throw new Error('API request failed');
            return response.status !== 204 ? await response.json() : null;
        }
        catch(e){
            console.log(e);
            throw e;
        }
        finally{
            setIsLoading(false);
        }
    }, [args, token, verb]);

    return [fetchApi, isLoading] as const;
}