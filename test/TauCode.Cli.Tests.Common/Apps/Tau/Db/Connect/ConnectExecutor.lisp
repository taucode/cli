(executor
    (key-value
        :keys ("-c" "--connection")
        :alias "connection"
        :token-types ("string")
    )

    (key-value
        :keys ("-p" "--provider")
        :alias "provider"
        :token-types ("db-provider")
    )
    
    (end)
)
